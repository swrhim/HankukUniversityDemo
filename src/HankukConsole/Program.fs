open System
open LLama.Common
open LLama

type ModelParam =
    { modelPath: string
      contextSize: int
      seed: int
      gpuLayerCount: int }

type Prompt =
| Stop
| Msg of string

let convertToDU = function
| "stop" -> Stop
| msg -> Msg msg

let convertToString = function
| Msg m -> m
| Stop -> "stop"

let modelPath = "/Users/drhim/dev/HankukUniversityDemo/models/llama-2-7b-chat.Q4_0.gguf"

let createModelParams modelParam =
    let modelParams =
            new Common.ModelParams(modelParam.modelPath)
    modelParams.ContextSize <- modelParam.contextSize
    modelParams.Seed <- modelParam.seed
    modelParams.GpuLayerCount <- modelParam.gpuLayerCount
    modelParams
    

let rec body (session : ChatSession) prompt =
    match prompt with
    | Stop -> ()
    | Msg msg -> 
        let inferenceParams = new InferenceParams()
        inferenceParams.Temperature <- 0.6f
        inferenceParams.AntiPrompts <- [ "User:" ]
        let text = session.Chat(msg, inferenceParams)
        
        text 
        |> Seq.map (fun x -> Console.Write(x)) 
        |> Seq.toList 
        |> ignore 
        
        Console.ForegroundColor <- ConsoleColor.Green
        let updatedPrompt = Console.ReadLine() |> convertToDU
        Console.ForegroundColor <- ConsoleColor.White

        body session updatedPrompt

[<EntryPoint>]
let main argv =

    let modelParam = { 
        modelPath = modelPath
        contextSize = 1024
        seed = 1337
        gpuLayerCount = 5 }

    let modelParams = createModelParams modelParam 
    
    use model = LLamaWeights.LoadFromFile(modelParams)
    use ctx = model.CreateContext(modelParams)
    let prompt = Msg "Hello! Feel free to ask me a question" 

    let ex = new InteractiveExecutor(ctx)
    let transformer = new LLamaTransforms.KeywordTextOutputStreamTransform([| "Users:"; "AI"|], 8)    
    let session = (new ChatSession(ex)).WithOutputTransform(transformer)

    Console.ForegroundColor <- ConsoleColor.Yellow
    Console.WriteLine("The chat session has started. The role names won't be printed.")
    Console.ForegroundColor <- ConsoleColor.White;
    Console.Write(convertToString prompt)
    
    body session prompt

    session.SaveSession("SavedSession")
    0
