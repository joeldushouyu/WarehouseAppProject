from OrderCommandParts.Command import  Command
from Database import loginedUsers

class LoginOrderCommand(Command):
    def __init__(self, currentUser, currentOrder):
        super().__init__(currentUser, currentOrder)




    def execute(self):
        #TODO: execute the command in user
        self.OnRelease()
        loginedUsers.append(self.currentUser) # add the current user into


    def interpretData(self):
        # TODO: interpret the data base on typs od command
        raise Exception("Function is not callable")

    def OnRelease(self):

        # there is nothing special to do at the moment of release
        pass