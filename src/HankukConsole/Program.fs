open System
open LLama.Common
open LLama

type ModelParam =
    { modelPath: string
      contextSize: int
      seed: int
      gpuLayerCount: int }

let modelPath = "C:\\dev\\HankukUniversityDemo\\models\\wizardLM-7B.ggmlv3.q4_1.bin"

let modelParam =
    { modelPath = modelPath
      contextSize = 1024
      seed = 1337
      gpuLayerCount = 5 }

[<EntryPoint>]
let main argv =
    let modelParams =
        new Common.ModelParams(modelParam.modelPath, modelParam.contextSize)

    let model = new LLamaModel(modelParams)

    let mutable prompt = "Hello! Feel free to ask me a question"

    let ex = new InteractiveExecutor(model)
    let session = new ChatSession(ex)
    Console.WriteLine()
    Console.Write(prompt)

    while not (prompt = "stop") do
        let inferenceParams = new InferenceParams()
        inferenceParams.Temperature <- 0.6f
        inferenceParams.AntiPrompts <- [ "User:" ]
        let text = session.Chat(prompt, inferenceParams)
        text |> Seq.map (fun x -> Console.Write(x)) |> Seq.toList |> ignore

        prompt <- Console.ReadLine()

    session.SaveSession("SavedSession")
    0
