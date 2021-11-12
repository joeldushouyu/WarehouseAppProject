class LocationComponent():
    def __init__(self,id = 0, description=""):
        self.id = id
        self.description = description

    def add(self, loc):
        raise Exception("interface do not implementt this funciton")
    def remove(self, loc):
        raise Exception("interface do not implement this function")