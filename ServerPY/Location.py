from LocationComponent import LocationComponent
#https://stackoverflow.com/questions/1167617/in-python-how-do-i-indicate-im-overriding-a-method
def overrides(interface_class):
    def overrider(method):
        assert(method.__name__ in dir(interface_class))
        return method
    return overrider

class Location(LocationComponent):

    def __init__(self):
        self.locationComponentList = []

    # add location to this. Note: only can have maxmium of 24 LocationComponent in list
    @overrides(LocationComponent)
    def add(self, loc:LocationComponent):
        if len(self.locationComponentList) < 24:
            self.locationComponentList.append(loc)
        else:
            raise IndexError("Only can have maxmium of 24 different locationComponent under 1 location")

    @overrides(LocationComponent)
    def remove(self,loc:LocationComponent):
        self.locationComponentList.remove(loc)


