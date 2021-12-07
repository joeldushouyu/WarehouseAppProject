import datetime
from datetime import date
from OrderCommandParts.Command import Command
from User import  User

class Session():
    def __init__(self, UserUUID:str, action:Command, time:datetime.date, currentUser:User):
        self.sessionUUID = UserUUID  # should be the uuid generate for each user upon login
        self.action = action
        self.time = time
        self.currentUser = currentUser

    def execute(self):

        self.action.execute()  # invoke the execute function in correspond command