# playback\_clip\_rect\_y #
## Summary ##

`playback_clip_rect_y` indicates the clipping region's top margin.

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
  <param name="playback_clip_rect_y" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Top Margin**                                 |
|:----------|:-----------------------------------------------|
| **Minimum** | `0`                                            |
| **Maximum** | `{osd_height} - {playback_clip_rect_height}`   |
| **Default** | `0`                                            |