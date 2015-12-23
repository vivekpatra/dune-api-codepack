# teletext\_enabled #
## Summary ##

`teletext_enabled` indicates whether to show teletext.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| teletext\_enabled  | {value}              |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="teletext_enabled" value="{value}"/>
  ...
</command_result>
```

## Values ##

|            | **Teletext** |
|:-----------|:-------------|
| **Enabled**  | `1`          |
| **Disabled** | `0`          |