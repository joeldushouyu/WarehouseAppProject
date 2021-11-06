import datetime
from Enumerables import NotificationType
class Notification():
    def __init__(self, type:NotificationType, message:str, date:datetime ):
        self.notificationType = type
        self.message = message
        self.notificationDate = date

