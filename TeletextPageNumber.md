# teletext\_page\_number #
## Summary ##

`teletext_page_number` indicates the current teletext page.

## Input ##

| **Parameter**          | **Value(s)**         |
|:-----------------------|:---------------------|
| cmd                    | set\_playback\_state |
| teletext\_page\_number | {n}                  |
| [[timeout](timeout.md)]          | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="teletext_page_number" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Page Number** |
|:----------|:----------------|
| **Minimum** | 100             |
| **Maximum** | 899             |