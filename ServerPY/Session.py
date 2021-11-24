import datetime
from datetime import date
from OrderCommandParts.Command import Command
from User import  User

class Session():
    def __init__(self, sessionUUID, action:Command, time:datetime.date, currentUser:User):
        self.sessionUUID = sessionUUID  # should be the uuid generate for each user upon login
        self.action = action
        self.time = time
        self.currentUser = currentUser

    def execute(self,data:dict):
        # TODO: actions when session got remove from the sessionList
        self.action.execute(data)