# video\_enabled #
## Summary ##

`video_enabled` indicates whether video output is enabled.

## Input ##

| **Parameter**        | **Value(s)**         |
|:---------------------|:---------------------|
| cmd                  | set\_playback\_state |
| video\_enabled       | {value}              |
| [[timeout](timeout.md)]        | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_enabled" value="{value}"/>
  ...
</command_result>
```

## Values ##

|           | **Video Enabled** |
|:----------|:------------------|
| **True**    | `1`               |
| **False**   | `0`               |
| **Default** | `1`               |

## Notes ##

  * When disabled, overlay graphics such as subtitles and pop up menus are still visible.

  * It is remarkable that this parameter is shown in the command results, but  [black\_screen](BlackScreen.md) and [hide\_osd](HideOsd.md) are not.

  * Setting both `video_enabled` and [hide\_osd](HideOsd.md) to 1 has almost the same effect as setting [black\_screen](BlackScreen.md) to 1. Subtitles will still be visible though.