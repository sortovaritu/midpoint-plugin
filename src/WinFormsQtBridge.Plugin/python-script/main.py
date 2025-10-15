import sys
import json
import msgpack
import zmq
from PyQt5.QtWidgets import QApplication, QWidget, QPushButton, QVBoxLayout, QPlainTextEdit, QLineEdit, QLabel
from PyQt5.QtCore import QThread, pyqtSignal

context = zmq.Context()
socket = context.socket(zmq.REQ)
socket.connect("tcp://127.0.0.1:5555")


class ZmqWorker(QThread):
    result_ready = pyqtSignal(object)

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

        self.multiplier_label = QLabel("Multiplier (целое число):")
        self.layout.addWidget(self.multiplier_label)
        self.multiplier_input = QLineEdit()
        self.multiplier_input.setText("2")
        self.layout.addWidget(self.multiplier_input)

        self.name_label = QLabel("Name:")
        self.layout.addWidget(self.name_label)
        self.name_input = QLineEdit()
        self.name_input.setText("New")
        self.layout.addWidget(self.name_input)

        self.btn1 = QPushButton("Get WellLog")
        self.layout.addWidget(self.btn1)

        self.worker = None  # ссылка на поток

        self.btn1.clicked.connect(self.on_btn_click)

    def handle_result(self, well_json):
        self.response_edit.clear()

        if isinstance(well_json, str):
            try:
                well_json = json.loads(well_json)
            except Exception as e:
                self.response_edit.appendPlainText(f"Error parsing server response: {e}")
                return

        if 'error' in well_json:
            self.response_edit.appendPlainText(f"Error: {well_json['error']}")
            return

        try:
            multiplier = int(self.multiplier_input.text())
        except ValueError:
            multiplier = 2

        for row in well_json['Log']:
            row['Value'] *= multiplier

        well_json['Name'] = self.name_input.text() or "New"

        self.response_edit.appendPlainText(json.dumps(well_json, indent=4))

        result = dict(Action='SetWellLog', Data=json.dumps(well_json, indent=4))
        socket.send_string(json.dumps(result))
        response_bytes = socket.recv()
        self.response_edit.appendPlainText("\nModified data sent back to server.")

    def on_btn_click(self):
        if self.worker is None or not self.worker.isRunning():
            self.worker = ZmqWorker({"Action": "GetWellLog"})
            self.worker.result_ready.connect(self.handle_result)

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
