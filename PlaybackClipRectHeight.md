# playback\_clip\_rect\_height #
## Summary ##

`playback_clip_rect_height` indicates the clipping region's height.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| clip\_rect\_x      | {n}                  |
| clip\_rect\_y      | {n}                  |
| clip\_rect\_width  | {n}                  |
| clip\_rect\_height | {n}                  |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_clip_rect_height" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Height**                                |
|:----------|:------------------------------------------|
| **Minimum** | `0`                                       |
| **Maximum** | `{osd_height} - {playback_clip_rect_y}`   |
| **Default** | `{osd_height}`                            |