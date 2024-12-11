function initialize() {
    const updateButtons = () => {
      const tasks = document.querySelectorAll("#list > div");
      tasks.forEach((task, index) => {
        const button = task.querySelector("button");
        if (index === 0 && button) {
          button.remove(); // Remove button from the top task
        } else if (index > 0 && !button) {
          const newButton = document.createElement("button");
          newButton.type = "button";
          newButton.innerHTML = "&uarr;";
          newButton.addEventListener("click", () => {
            const currentDiv = newButton.parentElement; // Get the current div
            const previousDiv = currentDiv.previousElementSibling; // Get the previous div
  
            if (previousDiv) {
              currentDiv.parentElement.insertBefore(currentDiv, previousDiv); // Move current div up
              updateButtons(); // Reapply button visibility rules
            }
          });
          task.appendChild(newButton); // Add button to non-top tasks
        }
      });
    };
  
    updateButtons();
  }
  
  
  initialize();
  document.getElementsByTagName("button")[1].click();
  document.getElementsByTagName("button")[0].click();
  
  // Monthly report should become the first task, followed by Read emails and Prepare presentation.
  console.log(document.getElementById('list').children);