function initialize() {
    const buttons = document.querySelectorAll("button");
    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        const currentDiv = button.parentElement; // Get the current div
        const previousDiv = currentDiv.previousElementSibling; // Get the previous div
  
        if (previousDiv) {
          currentDiv.parentElement.insertBefore(currentDiv, previousDiv); // Move current div up
        }
      });
    });
  }
  
  
  
  initialize();
  document.getElementsByTagName("button")[1].click();
  document.getElementsByTagName("button")[0].click();
  
  // Monthly report should become the first task, followed by Read emails and Prepare presentation.
  console.log(document.getElementById('list').children);