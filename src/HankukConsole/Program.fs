open System
open LLama.Common
open LLama

type ModelParam =
    { modelPath: string
      contextSize: int
      seed: int
      gpuLayerCount: int }

let modelPath = "/Users/drhim/dev/HankukUniversityDemo/models/llama-2-7b-chat.Q4_0.gguf"

let modelParam =
    { modelPath = modelPath
      contextSize = 1024
      seed = 1337
      gpuLayerCount = 2 }

let rec body (session : ChatSession) prompt =
    match prompt with
    | "stop" -> ()
    | _ -> 
        let inferenceParams = new InferenceParams()
        inferenceParams.Temperature <- 0.6f
        inferenceParams.AntiPrompts <- [ "User:" ]
        let text = session.Chat(prompt, inferenceParams)
        
        text 
        |> Seq.map (fun x -> Console.Write(x)) 
        |> Seq.toList 
        |> ignore 
        
        Console.ForegroundColor <- ConsoleColor.Green
        let updatedPrompt = Console.ReadLine()
        Console.ForegroundColor <- ConsoleColor.White

        body session updatedPrompt

[<EntryPoint>]
let main argv =
    let modelParams =
        new Common.ModelParams(modelParam.modelPath)
    modelParams.ContextSize <- modelParam.contextSize
    modelParams.Seed <- modelParam.seed
    modelParams.GpuLayerCount <- modelParam.gpuLayerCount
    
    use model = LLamaWeights.LoadFromFile(modelParams)
    use ctx = model.CreateContext(modelParams)
    let prompt = "Hello! Feel free to ask me a question"

    let ex = new InteractiveExecutor(ctx)
    let transformer = new LLamaTransforms.KeywordTextOutputStreamTransform([| "Users:"; "AI"|], 8)    
    let session = (new ChatSession(ex)).WithOutputTransform(transformer)

    Console.ForegroundColor <- ConsoleColor.Yellow
    Console.WriteLine("The chat session has started. The role names won't be printed.")
    Console.ForegroundColor <- ConsoleColor.White;
    Console.Write(prompt)
    
    body session prompt

    session.SaveSession("SavedSession")
    0
