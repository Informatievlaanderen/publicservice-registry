module PublicService.ActivePatterns

  open System.Text.RegularExpressions

  let (|DVR|_|) location =
    let m = Regex.Match(location, @"\/v[0-9]\/dienstverleningen\/(?<dvr>.*)")

    if (m.Success)
      then Some m.Groups.["dvr"].Value
      else None
