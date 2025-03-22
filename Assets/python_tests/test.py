# Test for stat generation

    # // Controls the main player stats
    # // Main stats (initial values) (average)
    # // Strength (2-10) (4)
    # // Intelligence (2-10) (4)
    # // Constitution (2-10) (4)
    # // Dexterity (2-10) (4)
    # // Charisma (2-10) (4)
    # //
    # // 10 == Max of natural human ability
    # // 2 == ~minimum for a human

import random
import numpy as np
from copy import deepcopy
import matplotlib.pyplot as plt

stats_historical = []
std_devs = []
means = []

stat_weights = {
    2:  1/60, # 1 in 60
    3:  1/16, # 1 in 16
    4:  1/4,
    5:  1/12,
    6:  1/80, 
    7:  1/750, 
    8:  1/5_000, 
    9:  1/10_000, 
    10: 1/100_000
}
total_average = 4

def randomize_stats():

    # Weighting stats
    # Measures stats as they are generated
    # Decreases chance of getting a high stat if average is high
    stats = {}
    _stat_weights = deepcopy(stat_weights)

    skills = ['strength', 'intelligence', 'constitution', 'dexterity', 'charisma']
    # shuffle skills
    random.shuffle(skills)

    for stat in skills:
        stats[stat] = random.choices(list(_stat_weights.keys()), list(_stat_weights.values()))[0]
        average = (sum(stats.values()) / len(stats))
        # Adjust weights
        for key in _stat_weights.keys():
            if key > average:
                _stat_weights[key] *= 1.5
            else:
                _stat_weights[key] *= 0.7
    return stats

for i in range(10000):
    stats = randomize_stats()
    mean = np.mean(list(stats.values()))
    std_dev = np.std(list(stats.values()))

    stats_historical.append(stats)
    std_devs.append(std_dev)
    means.append(mean)

print(f"Average Standard Deviation: {sum(std_devs) / len(std_devs)}")
print(f"Average Mean: {sum(means) / len(means)}")

# Plotting
fig, axs = plt.subplots(1, 5)
fig.set_size_inches(15, 5)
fig.suptitle('Stat Distribution')

for i, stat in enumerate(['strength', 'intelligence', 'constitution', 'dexterity', 'charisma']):
    axs[i].hist([stats[stat] for stats in stats_historical], bins=range(2, 12), align='left', rwidth=0.8)
    axs[i].set_title(stat)

    # Turn on grid
    axs[i].grid(True)

plt.show()