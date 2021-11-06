import datetime
from Command import Command
from User import  User
class Session():
    def __init__(self, sessionId, action:Command, time:datetime, currentUser:User):
        self.sessionId = sessionId
        self.action = action
        self.time = time
        self.currentUser = currentUser

    def Onrelease(self):
        # TODO: actions when session got remove from the sessionList
        pass