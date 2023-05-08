## Project description
A Creative Support Tool(CST) for creating and editing music using 3D block components such as notes, beats, and chords. The main tasks of the XR application includes modifying audio sound and pitch, adjusting the length of notes, creating and combining notes, playing notes, and visualizing them in a 3D space. It is a fun and new interactive way for users to create and edit music in a 3D space. 

## User instructions
Our notes are represented as colored, lit clouds with different colors corresponding to different notes (A-G: Red-Purple) and different lighting corresponding to different instruments (guitar/bass/xylophone)

Moving Around:
Use the two triggers and pull towards yourself using the right controller to travel. If not you can use the left controller’s analog stick to do continuous movement.

Toggle Help Menu:
Click the left controller menu button to pop open and close the help menu with control scheme.

Toggle Notes Menu:
Click the left controller joystick button to pop open and close the menu for choosing notes/instruments.

Placing Notes:
Have the menu open and point your ray casted from your controller and press the trigger on the note you want to create. Then click on the right controller’s analog stick to create the music note.

Changing Instruments:
Have the menu open and point your ray casted from your controller at the cycle symbol to cycle through the instruments.

Adjusting Volume:
Pitch can be controlled by the user through bimanual interaction (y distance based and discrete) using left trigger and right trigger and changing the controller Y distance. 

Adjusting Pitch:
Pitch can be controlled by the user through bimanual interaction (x distance based and discrete) using left secondary and right secondary and changing the controller X distance. 

Connect Sequentially:
Hold the trigger and secondary button on the right controller while the raycast is hitting a specific note and dragging that to another note. 

Unconnecting/Removing Sequentially:
Hold the trigger and secondary button on the left controller while the raycast is hitting a specific note and dragging that to another note.

Playing:
With the ray cast from the controller hitting the note, right primary button. Particles will shoot out of the notes when they are playing. If the notes aren’t playing, then no particles will appear. If connected the speed at which each note will play after another is based on the distance of the line renderer in between each.

Delete Note:
With the ray cast from the controller hitting the note, left primary button.



## Interaction techniques
We developed a symmetric-synchronous bimanual technique. We use two 6-DOF handheld controllers that allow users to adjust the pitch and volume of a note block. This is done by referencing the two controllers’ positions and calculating the distance along an axis.

We incorporated an adapted 3D UI Menu which was well suited for providing good structure for creating notes for users. Used a sort of U-radial menu which is somewhat transparent but can occlude some of the environment.

We also included a simple sweep gesture for connecting notes to each other with a Line Renderer that was colored with a rainbow spectrum and would start with a wide width and end with a thin width in order to show the forward direction of the sequence.

For traveling we created a hybrid of continuous movement and grab move. The reason for this was rotation with grab move caused vection forcing users to take a break from the Creative Support Tool. So we combined the translation of grab move with continuous movement for precise maneuvering and search tasks. 

## Work summary
During the development of our application, we faced several challenges. Firstly, we had to learn how to import and use 3D models from external sources such as turbosquid.com. Additionally, we had to brainstorm interesting visual feedback techniques to enhance the user experience, such as using particle filters. Another challenge we faced was finding a way to visually connect notes together using line renderers to provide better feedback and enhance the overall aesthetic of the application. We enjoyed being able to see which interaction techniques were well suited for our application’s purpose. We didn’t enjoy the design process for creating UI menus and feedback. We learned how to quickly implement interaction techniques using the XR toolkit. If we had more time we would like to implement more audio cues to create greater immersion.
