from Command import  Command

class ReleaseOrderCommand(Command):
    def __init__(self, currentUser, currentOrder):
        super().__init__(currentUser, currentOrder)




    def execute(self, data:dict):
        #TODO: execute the command in user
        pass

    def interpretData(self):
        # TODO: interpret the data base on typs od command
        pass
    def OnRelease(self):
        # TODO: calls when the command is going to be release and delete
        pass