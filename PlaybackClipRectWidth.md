# playback\_clip\_rect\_width #
## Summary ##

`playback_clip_rect_width` indicates the clipping region's width.

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
  <param name="playback_clip_rect_width" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Width**                                |
|:----------|:-----------------------------------------|
| **Minimum** | `0`                                      |
| **Maximum** | `{osd_width} - {playback_clip_rect_x}`   |
| **Default** | `{osd_width}`                            |