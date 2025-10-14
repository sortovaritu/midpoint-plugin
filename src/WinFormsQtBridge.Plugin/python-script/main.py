import sys
import json
import msgpack
import zmq
from PyQt5.QtWidgets import QApplication, QWidget, QPushButton, QVBoxLayout, QPlainTextEdit
from PyQt5.QtCore import QThread, pyqtSignal

context = zmq.Context()
socket = context.socket(zmq.REQ)
socket.connect("tcp://127.0.0.1:5555")


class ZmqWorker(QThread):
    result_ready = pyqtSignal(str)

    def __init__(self, request):
        super().__init__()
        self.request = request

    def run(self):
        try:
            socket.send_string(json.dumps(self.request))
            response_bytes = socket.recv()
            well = msgpack.unpackb(response_bytes, raw=False)
            self.result_ready.emit(well)
        except Exception as e:
            self.result_ready.emit({"error": str(e)})


class MainWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Qt ZeroMQ Client")
        self.layout = QVBoxLayout()
        self.setLayout(self.layout)

        self.response_edit = QPlainTextEdit()
        self.response_edit.setFixedHeight(600)
        self.response_edit.setFixedWidth(400)
        self.layout.addWidget(self.response_edit)

        self.btn1 = QPushButton("Get WellLog")
        self.layout.addWidget(self.btn1)

        self.worker = None  # поток

        # подключение кнопки **после того как метод определён**
        self.btn1.clicked.connect(self.on_btn_click)

    def handle_result(self, well):
        self.response_edit.clear()
        well_json = json.loads(well)
        
        for row in well_json['Log']:
            row['Value'] *= 2
        
        well_json['Name'] = 'New'
        
        self.response_edit.appendPlainText(json.dumps(well_json, indent=4))
        
        result = dict(Action='SetWellLog', Data=json.dumps(well_json, indent=4))
        
        socket.send_string(json.dumps(result))
        response_bytes = socket.recv()
        self.response_edit.appendPlainText("\nModified data sent back to server.")

    def on_btn_click(self):
        # создаём поток только если нет активного
        if self.worker is None or not self.worker.isRunning():
            self.worker = ZmqWorker({"Action": "GetWellLog"})
            self.worker.result_ready.connect(self.handle_result)

            # После завершения очищаем ссылку
            def cleanup():
                self.worker.deleteLater()
                self.worker = None

            self.worker.finished.connect(cleanup)
            self.worker.start()


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec_())