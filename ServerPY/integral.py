import numpy as np
from scipy.integrate import dblquad


from scipy import integrate

def f(x, y):

    return y**2*( (x-2*y) / ((x+y)**2)  )


def bounds_y():

    return [-2/3, 8/3]


def bounds_x(y):

    return [1/3, 8/3]


integrate.nquad(f, [bounds_x, bounds_y])