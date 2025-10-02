import sys
import win32file
from PyQt5.QtWidgets import QApplication, QWidget, QPushButton, QVBoxLayout, QLineEdit

PIPE_NAME = r'\\.\pipe\PluginPipe'

def send_message(message):
    try:
        handle = win32file.CreateFile(
            PIPE_NAME,
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None,
            win32file.OPEN_EXISTING,
            0, None
        )
        # отправляем сообщение
        win32file.WriteFile(handle, (message + "\n").encode('utf-8'))
        # читаем ответ
        resp = win32file.ReadFile(handle, 4096)[1]
        response_text = resp.decode('utf-8')

        # отображаем в edit поле
        response_edit.setText(response_text)

        handle.Close()
    except Exception as e:
        response_edit.setText(f"Ошибка: {e}")

app = QApplication([])

window = QWidget()
window.setWindowTitle("Qt NamedPipe Client")

layout = QVBoxLayout()

# Кнопки
btn1 = QPushButton("Button1")
btn1.clicked.connect(lambda: send_message("Button1"))

btn2 = QPushButton("Button2")
btn2.clicked.connect(lambda: send_message("Button2"))

# Поле для отображения ответа
response_edit = QLineEdit()
response_edit.setReadOnly(True)

# Добавляем элементы в layout
layout.addWidget(btn1)
layout.addWidget(btn2)
layout.addWidget(response_edit)

window.setLayout(layout)
window.show()
app.exec_()