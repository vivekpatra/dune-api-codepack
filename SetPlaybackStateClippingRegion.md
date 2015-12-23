# set\_playback\_state: Clipping Region #
## Summary ##

The following parameters are used to set a clipping region.

Graphics outside the clipping rectangle are not rendered on screen.

In order to set a custom clipping rectangle, you must specify all four parameters with proper values.

## Syntax ##

| **Parameter**                               | **Value(s)**         | **Example**              | **API version(s)** |
|:--------------------------------------------|:---------------------|:-------------------------|:-------------------|
| [cmd](Cmd.md)                                 | set\_playback\_state | cmd=set\_playback\_state | ≥ 1                |
| [clip\_rect\_x](PlaybackClipRectX.md)           | {n}                  | clip\_rect\_x=192        | ≥ 3                |
| [clip\_rect\_y](PlaybackClipRectY.md)           | {n}                  | clip\_rect\_y=108        | ≥ 3                |
| [clip\_rect\_width](PlaybackClipRectWidth.md)   | {n}                  | clip\_rect\_width=1536   | ≥ 3                |
| [clip\_rect\_height](PlaybackClipRectHeight.md) | {n}                  | clip\_rect\_height=864   | ≥ 3                |
| [[timeout](Timeout.md)]                       | {n}                  | timeout=20               | ≥ 1                |

### Examples ###

Show only the **left** half of the screen, display size is 1080p (1920×1080):

`http://dune/cgi-bin/do?cmd=set_playback_state&clip_rect_x=0&clip_rect_y=0&clip_rect_width=960&clip_rect_height=1080`


---


Show only the **right** half of the screen, display size is 1080p (1920× 1080):

`http://dune/cgi-bin/do?cmd=set_playback_state&clip_rect_x=960&clip_rect_y=0&clip_rect_width=960&clip_rect_height=1080`