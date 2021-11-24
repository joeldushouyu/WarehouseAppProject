# intends to use as a interface for all of the commands
import User
import Order

class Command():


    def __init__(self, currentUser:User, order):
        self.currentUser = currentUser # the user that current holds this
        self.correspondOrder = order

    def execute(self, data:dict):
        #TODO: execute the command in user
        pass

    def interpretData(self):
        # TODO: interpret the data base on typs od command
        pass
    def OnRelease(self):
        # TODO: calls when the command is going to be release and delete
        pass

