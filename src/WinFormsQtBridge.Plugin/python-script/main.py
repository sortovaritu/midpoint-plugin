import sys
import win32file
from PyQt5.QtWidgets import QApplication, QWidget, QPushButton, QVBoxLayout, QPlainTextEdit

PIPE_NAME = r'\\.\pipe\WinFormsQtBridge.PipeServerService'

def send_message(message):
    try:
        handle = win32file.CreateFile(
            PIPE_NAME,
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None,
            win32file.OPEN_EXISTING,
            0, None
        )

        win32file.WriteFile(handle, (message + "\n").encode('utf-8'))

        resp = win32file.ReadFile(handle, 4096)[1]
        response_text = resp.decode('utf-8')


        response_edit.setPlainText(response_text)

        handle.Close()
    except Exception as e:
        response_edit.setPlainText(f"Ошибка: {e}")

app = QApplication([])

window = QWidget()
window.setWindowTitle("Qt NamedPipe Client")

layout = QVBoxLayout()


btn1 = QPushButton("Get WellLog")
btn1.clicked.connect(lambda: send_message('{ "Action": "Start" }'))

response_edit = QPlainTextEdit()
response_edit.setFixedHeight(100)
layout.addWidget(response_edit)


layout.addWidget(btn1)
layout.addWidget(response_edit)

window.setLayout(layout)
window.show()
app.exec_()