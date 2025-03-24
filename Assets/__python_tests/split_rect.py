import matplotlib.pyplot as plt

grid_size = 256
grid_bits = [[0 for _ in range(grid_size)] for _ in range(grid_size)]

room_size = 50
room_start = [50, 50]

for i in range(room_size):
    for j in range(room_size):
        grid_bits[room_start[0] + i][room_start[1] + j] = 1

# There will always be 4 rectangles around the middle room
# First we get the rectangles without respect to the others


# First top, right, bottom, left
# Top
# x1 = 0
# y1 = 0
# x2 = grid_size
# y2 = room_start[1]

rect_top = {"x1": 0, "y1": 0, "x2": grid_size, "y2": room_start[1]}
rect_right = {"x1": room_start[0] + room_size, "y1": 0, "x2": grid_size, "y2": grid_size}
rect_bottom = {"x1": 0, "y1": room_start[1] + room_size, "x2": grid_size, "y2": grid_size}
rect_left = {"x1": 0, "y1": 0, "x2": room_start[0], "y2": grid_size}

# Priority, top bottom rects

# Left and right will be shortened by the top and bottom rects
rect_right["y1"] = rect_top["y2"]
rect_right["y2"] = rect_bottom["y1"]

rect_left["y1"] = rect_top["y2"]
rect_left["y2"] = rect_bottom["y1"]


print(rect_top)
print(rect_bottom)
print(rect_right)
print(rect_left)
plt.plot([rect_top["x1"], rect_top["x2"]], [rect_top["y1"], rect_top["y1"]], color="red", linewidth=2)
plt.plot([rect_top["x1"], rect_top["x2"]], [rect_top["y2"], rect_top["y2"]], color="red", linewidth=2)
plt.plot([rect_top["x1"], rect_top["x1"]], [rect_top["y1"], rect_top["y2"]], color="red", linewidth=2)
plt.plot([rect_top["x2"], rect_top["x2"]], [rect_top["y1"], rect_top["y2"]], color="red", linewidth=2)
plt.plot([rect_bottom["x1"], rect_bottom["x2"]], [rect_bottom["y1"], rect_bottom["y1"]], color="red", linewidth=2)
plt.plot([rect_bottom["x1"], rect_bottom["x2"]], [rect_bottom["y2"], rect_bottom["y2"]], color="red", linewidth=2)
plt.plot([rect_bottom["x1"], rect_bottom["x1"]], [rect_bottom["y1"], rect_bottom["y2"]], color="red", linewidth=2)
plt.plot([rect_bottom["x2"], rect_bottom["x2"]], [rect_bottom["y1"], rect_bottom["y2"]], color="red", linewidth=2)

plt.plot([rect_right["x1"], rect_right["x2"]], [rect_right["y1"], rect_right["y1"]], color="red", linewidth=2)
plt.plot([rect_right["x1"], rect_right["x2"]], [rect_right["y2"], rect_right["y2"]], color="red", linewidth=2)
plt.plot([rect_right["x1"], rect_right["x1"]], [rect_right["y1"], rect_right["y2"]], color="red", linewidth=2)
plt.plot([rect_right["x2"], rect_right["x2"]], [rect_right["y1"], rect_right["y2"]], color="red", linewidth=2)
plt.plot([rect_left["x1"], rect_left["x2"]], [rect_left["y1"], rect_left["y1"]], color="red", linewidth=2)
plt.plot([rect_left["x1"], rect_left["x2"]], [rect_left["y2"], rect_left["y2"]], color="red", linewidth=2)
plt.plot([rect_left["x1"], rect_left["x1"]], [rect_left["y1"], rect_left["y2"]], color="red", linewidth=2)
plt.plot([rect_left["x2"], rect_left["x2"]], [rect_left["y1"], rect_left["y2"]], color="red", linewidth=2)

plt.imshow(grid_bits, cmap="gray")
plt.show()

rects = [rect_top, rect_bottom, rect_right, rect_left]
# rects = [{"x1": 0, "y1": 0, "x2": 20, "y2": 40}, {"x1": 0, "y1": 40, "x2": 30, "y2": 50}, {"x1": 25, "y1": 0, "x2": 30, "y2": 40}]
new_rects = []
vertical_lines = [12]
# Vertical line at 12 
# Check if the line is within the rectangle
for line in vertical_lines:
    for ix, rect in enumerate(rects):
        if rect["x1"] < line < rect["x2"]:
            print(ix,end=" ")
            print("VLine intersects the rectangle")
            new_rects.append({"x1": rect["x1"], "y1": rect["y1"], "x2": line, "y2": rect["y2"]})
            new_rects.append({"x1": line, "y1": rect["y1"], "x2": rect["x2"], "y2": rect["y2"]})            

horizontal_lines = [13, 27]

for line in horizontal_lines:
    for ix, rect in enumerate(rects):
        if rect["y1"] < line < rect["y2"]:
            print(ix,end=" ")
            print("HLine intersects the rectangle")
            new_rects.append({"x1": rect["x1"], "y1": rect["y1"], "x2": rect["x2"], "y2": line})
            new_rects.append({"x1": rect["x1"], "y1": line, "x2": rect["x2"], "y2": rect["y2"]})

plt.figure()

# Plot the rectangle
for rect in rects:
    plt.plot([rect["x1"], rect["x2"]], [rect["y1"], rect["y1"]], color="red", linewidth=2)
    plt.plot([rect["x1"], rect["x2"]], [rect["y2"], rect["y2"]], color="red", linewidth=2)
    plt.plot([rect["x1"], rect["x1"]], [rect["y1"], rect["y2"]], color="red", linewidth=2)
    plt.plot([rect["x2"], rect["x2"]], [rect["y1"], rect["y2"]], color="red", linewidth=2)

# Plot the new rectangles
for rect in new_rects:
    plt.plot([rect["x1"], rect["x2"]], [rect["y1"], rect["y1"]], color="green", linewidth=2)
    plt.plot([rect["x1"], rect["x2"]], [rect["y2"], rect["y2"]], color="green", linewidth=2)
    plt.plot([rect["x1"], rect["x1"]], [rect["y1"], rect["y2"]], color="green", linewidth=2)
    plt.plot([rect["x2"], rect["x2"]], [rect["y1"], rect["y2"]], color="green", linewidth=2)

# Plot the horizontal lines
for line in horizontal_lines:
    plt.plot([0, 255], [line, line], color="blue", linewidth=2)

# Plot the vertical lines
for line in vertical_lines:
    plt.plot([line, line], [0, 255], color="blue", linewidth=2)
plt.show()