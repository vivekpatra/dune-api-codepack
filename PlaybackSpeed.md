# playback\_speed #
## Summary ##

`playback_speed` indicates the current playback speed.
It is expressed as a numeric value ranging from 2<sup>3</sup> to 2<sup>13</sup>, positive or negative, or 0. Positive means forwarding while negative values mean rewinding.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| speed         | {n}                  |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_speed" value="{n}"/>
  ...
</command_result>
```

## Values ##

|                                                                       | **Speed** | **API Version(s)** |
|:----------------------------------------------------------------------|:----------|:-------------------|
| <font size='3' face='webdings'>7</font> x32| `-8192`   | ≥ 1                |
| <font size='3' face='webdings'>7</font> x16| `-4096`   | ≥ 1                |
| <font size='3' face='webdings'>7</font> x8| `-2048`   | ≥ 1                |
| <font size='3' face='webdings'>7</font> x4| `-1024`   | ≥ 1                |
| <font size='3' face='webdings'>7</font> x2| `-512`    | ≥ 1                |
| <font size='3' face='webdings'>3</font>| `-256`    | ≥ 1                |
| <font size='3' face='webdings'>3</font> x<sup>1</sup>/<sub>2</sub>| `-128`    | ≥ 1                |
| <font size='3' face='webdings'>3</font> x<sup>1</sup>/<sub>4</sub>| `-64`     | ≥ 1                |
| <font size='3' face='webdings'>3</font> x<sup>1</sup>/<sub>8</sub>| `-32`     | ≥ 1                |
| <font size='3' face='webdings'>3</font> x<sup>1</sup>/<sub>16</sub>| `-16`     | ≥ 1                |
| <font size='3' face='webdings'>3</font> x<sup>1</sup>/<sub>32</sub>| `-8`      | ≥ 3                |
| <font size='3' face='webdings'>;</font>| `0`       | ≥ 1                |
| <font size='3' face='webdings'>4</font> x<sup>1</sup>/<sub>32</sub>| `8`       | ≥ 3                |
| <font size='3' face='webdings'>4</font> x<sup>1</sup>/<sub>16</sub>| `16`      | ≥ 1                |
| <font size='3' face='webdings'>4</font> x<sup>1</sup>/<sub>8</sub>| `32`      | ≥ 1                |
| <font size='3' face='webdings'>4</font> x<sup>1</sup>/<sub>4</sub>| `64`      | ≥ 1                |
| <font size='3' face='webdings'>4</font> x<sup>1</sup>/<sub>2</sub>| `128`     | ≥ 1                |
| <font size='3' face='webdings'>4</font>| `256`     | ≥ 1                |
| <font size='3' face='webdings'>8</font> x2| `512`     | ≥ 1                |
| <font size='3' face='webdings'>8</font> x4| `1028`    | ≥ 1                |
| <font size='3' face='webdings'>8</font> x8| `2048`    | ≥ 1                |
| <font size='3' face='webdings'>8</font> x16| `4096`    | ≥ 1                |
| <font size='3' face='webdings'>8</font> x32| `8192`    | ≥ 1                |
| **Default**                                                             | `256`     | ≥ 1                |

## Notes ##

versions 1 through 3 return a bugged value when playback\_speed is between -128 and -8.

This bug has been fixed starting with firmware version 130429\_1605\_b6.