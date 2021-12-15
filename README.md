# [Advent Of Code 2021](https://adventofcode.com/2021)

Run in linqpad 7

~Not trying to be elegant or efficient, just having fun so don't judge too harshly.~

Not aiming for perfection, just having fun so don't judge too harshly.

That said I really wasn't happy with my naive first attempt for day 5 which ran for ~8 seconds so I guess I do care
a little about optimizing for runtime.


## Results

| Day | Both parts included | Runtime | Comments                                                                                                                      | Cheated            |
|-----|---------------------|---------|-------------------------------------------------------------------------------------------------------------------------------|--------------------|
| 1   | :heavy_check_mark:  | 0.001 s |                                                                                                                               | :x:                |
| 2   | :x:                 | 0.001 s | Overwrote part 1, before I was planning on sharing                                                                            | :x:                |
| 3   | :x:                 | 0.007 s | Overwrote part 1, before I was planning on sharing                                                                            | :x:                |
| 4   | :heavy_check_mark:  | 0.033 s |                                                                                                                               | :x:                |
| 5   | :heavy_check_mark:  | 0.046 s | First attempt took ~8 seconds to run for part 1                                                                               | :x:                |
| 6   | :heavy_check_mark:  | 0.001 s | First attempt out of memory part 2, also very long runtime                                                                    | :x:                |
| 7   | :heavy_check_mark:  | 0.18 s  | Pretty good first try, could probably cleanup but I'm happy.                                                                  | :x:                |
| 8   | :heavy_check_mark:  | 0.006 s | That was a right PITA                                                                                                         | :x:                |
| 9   | :heavy_check_mark:  | 0.010 s | Rushed catching up, probably much better ways to solve.                                                                       | :x:                |
| 10  | :heavy_check_mark:  | 0.001 s | Sounded more fun than it was.                                                                                                 | :x:                |
| 11  | :heavy_check_mark:  | 0.035 s | Rushed catching up, probably much better ways to solve.                                                                       | :x:                |
| 12  | :heavy_check_mark:  | ~6.5 s  | Didn't enjoy this one.                                                                                                        | :x:                |
| 13  | :heavy_check_mark:  | 0.001 s | Day 13 was really good fun!!!                                                                                                 | :x:                |
| 14  | :heavy_check_mark:  | 0.001 s | I didn't learn my lesson and ran out of memory first attempt at part 2.                                                       | :x:                |
| 15  | :heavy_check_mark:  | ~0.4 s  | Googled AStar which was first thing that came to mind, eventually pretty much stole someone else implementation for Dijkstra. | :heavy_check_mark: |


### Day 13 plotted.
```
#  #  ##  #    #### ###   ##  #### #  #
#  # #  # #       # #  # #  #    # #  #
#  # #    #      #  #  # #  #   #  #  #
#  # #    #     #   ###  ####  #   #  #
#  # #  # #    #    # #  #  # #    #  #
 ##   ##  #### #### #  # #  # ####  ## 
```