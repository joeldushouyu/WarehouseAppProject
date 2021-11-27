import datetime
from datetime import date
from OrderCommandParts.Command import Command
from User import  User

class Session():
    def __init__(self, UserUUID, action:Command, time:datetime.date, currentUser:User):
        self.sessionUUID = UserUUID  # should be the uuid generate for each user upon login
        self.action = action
        self.time = time
        self.currentUser = currentUser

    def execute(self):
        # TODO: actions when session got remove from the sessionList
        self.action.execute()