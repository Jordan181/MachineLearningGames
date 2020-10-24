# MachineLearningGames
A framework for training feed forward neural networks using a genetic learning algorithm. The framework is applied to three simple 2D games: Flappy Bird, Snake and a custom 
driving game.

The three games can be run by running the associated executable after building the solution, or by setting the appropriate starting project in Visual Studio.

Each game project contains a Program.cs file in which the controller is specified. By default, each game is set up to run with an AI controller using a neural network that has
been pre-trained and serialized. The games can be played using the keyboard by changing which implementation of the I<*>Controller interface is passed into the game constructor.

To train Neural Networks from scratch, run the AiTrainer project. The game which NNs are being trained for can be set by changing the Games enum value being passed to the 
CreateAiTrainer() method in Program.cs. Each game has its own Trainer class inheriting from the game class. These trainer classes contain specific instructions such as calculating
the fitness score for each NN at the end of the generation and recording any information at each cycle by overriding the Update method. However, the training is entirely
managed by the AiTrainingFramework class which in turn uses the GeneticAlgorithm.
