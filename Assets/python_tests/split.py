split_money = 30
take_money = 10
ops = 7


import random
import matplotlib.pyplot as plt

def split(split_money, ops):
    splitters = random.randint(1, ops)
    split = split_money / splitters
    return split

split_hist = []

for i in range(10000):
    var = split(split_money, ops)
    _var = take_money - var
    split_hist.append(_var)

plt.hist(split_hist, bins=20)
plt.show()
