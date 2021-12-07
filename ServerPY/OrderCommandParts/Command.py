# intends to use as a interface for all of the commands
import User
import Order

class Command():


    def __init__(self, currentUser:User, order):
        self.currentUser = currentUser # the user that current holds this
        self.correspondOrder = order

    def execute(self):

        pass

    def interpretData(self):

        pass
    def OnRelease(self):

        pass

