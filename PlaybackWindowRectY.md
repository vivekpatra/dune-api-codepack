# playback\_window\_rect\_y #
## Summary ##

`playback_window_rect_y` indicates the playback window rectangle's top margin.

## Input ##

| **Parameter**        | **Value(s)**         |
|:---------------------|:---------------------|
| cmd                  | set\_playback\_state |
| window\_fullscreen   | 0                    |
| window\_rect\_x      | {n}                  |
| window\_rect\_y      | {n}                  |
| window\_rect\_width  | {n}                  |
| window\_rect\_height | {n}                  |
| [[timeout](timeout.md)]        | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_window_rect_y" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Top Margin**                                   |
|:----------|:-------------------------------------------------|
| **Minimum** | `0`                                              |
| **Maximum** | `{osd_height} - {playback_window_rect_height}`   |
| **Default** | `0`                                              |

## Notes ##

This parameter was previously named [video\_y](VideoY.md) in version 2.