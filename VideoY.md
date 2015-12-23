# video\_y #
## Summary ##

`video_y` indicates the playback window rectangle's top margin.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| video\_fullscreen  | 0                    |
| video\_x           | {n}                  |
| video\_y           | {n}                  |
| video\_width       | {n}                  |
| video\_height      | {n}                  |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_y" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Top Margin**                                    |
|:----------|:--------------------------------------------------|
| **Minimum** | `0`                                               |
| **Maximum** | `{video_total_display_height} - {video_height}`   |
| **Default** | `0`                                               |

## Notes ##

This parameter has been renamed to [playback\_window\_rect\_y](PlaybackWindowRectY.md) starting with version 3.