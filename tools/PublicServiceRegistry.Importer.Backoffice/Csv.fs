module PublicService.Csv

  open FSharp.Data

  [<Literal>]
  let batchFile = "batch_20190220_sarah_altname_ipdc.txt"

  type PublicServices =
    CsvProvider<Sample      = batchFile,
                Separators  = "|",
                HasHeaders  = true,
                Encoding    = "utf-8"
                >
