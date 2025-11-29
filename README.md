# DDEmo - Emotional Interactive Story Application

This is an interactive emotional story application developed with Unity, designed to help users understand and express various emotions through the adventure story of a little penguin named Pip. The application integrates text-to-speech, animation control, and network communication features, supporting real-time feedback for emotion recognition and posture analysis.

## Project Features

- **Interactive Story Experience**: Follow little penguin Pip's adventure journey and experience different emotional states
- **Text-to-Speech**: Implement character voice dialogue using iFlytek TTS technology
- **Emotion Recognition**: Receive external emotion analysis data through TCP communication and provide feedback
- **Animation Control System**: Trigger corresponding character animations based on story progression and user feedback
- **Progress Tracking**: Real-time display of story progress and user performance ratings
- **Multiple Emotion Expressions**: Includes happy, surprised, scared, sad, angry, disgusted, and other emotional scenarios

## Technology Stack

- **Development Engine**: Unity 3D
- **Programming Language**: C#
- **Speech Technology**: iFlytek Speech Synthesis (XunFeiSpeech.TTS)
- **Network Communication**: TCP client/server architecture
- **Animation System**: Unity Animation
- **UI System**: Unity UI

## Project Structure

```
Assets/
├── Fonts/           # Font resources
├── Images/          # Image resources (backgrounds, text bubbles, etc.)
├── Models/          # 3D model resources
├── Musics/          # Music resources
├── Resources/       # General resources (expressions, etc.)
├── Scenes/          # Game scenes
└── Scripts/         # C# scripts
    ├── MainScript.cs         # Main script entry point
    ├── SpeechSpeak.cs        # Speech synthesis and story control
    ├── TCPClient.cs          # TCP network communication
    ├── TcpServer.cs          # TCP server implementation
    └── Other auxiliary scripts
```

## Core Function Modules

### 1. Story Control System (SpeechSpeak.cs)

- Manage story playback flow and dialogue sequences
- Control character animations and expression changes
- Process user emotion scores and feedback
- Display story progress and evaluations

### 2. Network Communication Module (TCPClient.cs)

- Establish TCP connection with external emotion analysis system
- Send and receive real-time emotion data
- Process received messages in Unity's main thread

### 3. Animation Control

- Switch character expressions and movements based on story progression
- Implement text bubble display effects
- Control scene transitions and effects

## Installation Instructions

1. Clone or download the project to your local machine
2. Open the project with Unity (version 2019.4 or higher recommended)
3. Ensure necessary dependency packages are installed (especially iFlytek Speech SDK)
4. Build and run the project in Unity Editor

## Usage

1. Start the application
2. Follow the story progression of little penguin Pip
3. Express corresponding emotions according to story prompts
4. The system will receive emotion analysis data through TCP and provide real-time feedback
5. After completing all emotional scenarios, you can view your final score

## Network Configuration

The application connects to a local TCP server (127.0.0.1:8080) by default. To modify connection settings, adjust the following parameters in the TCPClient component:

- `serverIP`: Server IP address
- `serverPort`: Server port number

## Data Format

The emotion analysis data received by the application is in JSON format, example:

```json
{
  "audio": {
    "emotion": "happy",
    "score": 0.95
  },
  "pose": {
    "actions": ["Happy"]
  }
}
```

## Notes

1. Ensure the external emotion analysis system is running and listening on the correct port before use
2. The application will generate Logs.txt in the StreamingAssets directory to record received data
3. If network connection fails, the application will not be able to receive emotion analysis feedback

## Extension Development

To add new story scenes or emotion types, modify the relevant methods in SpeechSpeak.cs, especially the following parts:

- OnCallBack method: Handles callbacks after speech playback completes
- ReciveScore method: Processes received emotion scores
- EnterFirstSpeek and EnterQiESpeek coroutines: Controls story flow switching

---

## License

[MIT License](https://opensource.org/licenses/MIT)

## Contact

For questions or suggestions, please contact the project maintainer.
