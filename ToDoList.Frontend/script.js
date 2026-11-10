const apiUrl = "https://localhost:7232/api/ToDoList";



const titleInput = document.getElementById("newTitle");
const addButton = document.getElementById("addButton");


if (titleInput && addButton) {
    addButton.disabled = titleInput.value.trim() === "";


    titleInput.addEventListener("input", function() {

        addButton.disabled = titleInput.value.trim() === "";
    });
}



// GET /api/tasks
async function loadTasks() {
	
	//sortowanie
	const sortValue = document.getElementById("sortSelect").value;

	
 let url = apiUrl;
  
  //warunek sprawdzający sortowanie
  if (sortValue) {
    url += `?sortBy=${sortValue}`;
  }

  try {
    logRequest("GET", url);

    const response = await fetch(url);
    const data = await readResponse(response);

    logResponse(response, data);

    if (!response.ok) {
      return;
    }

    showTasks(data);
  } catch (error) {
    logError(error);
  }
}

// GET /ToDoList/{id} — szczegóły są pobierane z API, nie z listy.
async function showDetails(id) {
  document.getElementById("listView").hidden = true;
  document.getElementById("detailsView").hidden = false;
  document.getElementById("taskDetails").hidden = true;
 // document.getElementById("detailsCompleted").textContent = task.isCompleted ? "Tak" : "Nie";
  const status = document.getElementById("detailsStatus");
  status.textContent = "Ładowanie…";
  const url = `${apiUrl}/${id}`;

  try {
    logRequest("GET", url);
    const response = await fetch(url);
    const task = await readResponse(response);
    logResponse(response, task);

    if (response.status === 404) {
      status.textContent = "Nie znaleziono zadania.";
      return;
    }

    if (!response.ok) {
      status.textContent = `Nie udało się pobrać zadania (HTTP ${response.status}).`;
      return;
    }

    document.getElementById("detailsId").textContent = task.id;
    document.getElementById("detailsName").textContent = task.nazwa;
  //  document.getElementById("taskDetails").hidden = false;
	document.getElementById("detailsCompleted").textContent = task.isCompleted ? "Tak" : "Nie";
	
	let priorityText = "Brak priorytetu";
    if (task.priority === 1) priorityText = "Niski";
    else if (task.priority === 2) priorityText = "Średni";
    else if (task.priority === 3) priorityText = "Wysoki";
    
    document.getElementById("detailsPriority").textContent = priorityText;

    document.getElementById("taskDetails").hidden = false;
    status.textContent = "";
  } catch (error) {
    status.textContent = "Nie udało się połączyć z API.";
    logError(error);
  }
}

function showList() {
  document.getElementById("detailsView").hidden = true;
  document.getElementById("listView").hidden = false;
  loadTasks();
}

// POST /api/tasks
async function addTask() {
  const nazwa = document.getElementById("newTitle").value;
  
// pobieramy wartość priorytety z nowego pola typu ComboBox w formularzu ToDoList

const prioritySelect = document.getElementById("newPriority");
const priorityValue = prioritySelect && prioritySelect.value !== "" ? parseInt(prioritySelect.value) : null;



  const task = {
    nazwa: nazwa,
	// dodajemy pole priority do obiektu wysyłanego do api/ToDoList
	priority: priorityValue,
  };

  const url = apiUrl;

  try {
    logRequest("POST", url, task);

    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(task),
    });

    const data = await readResponse(response);

    logResponse(response, data);

    if (!response.ok) {
      return;
    }

    document.getElementById("newTitle").value = "";
	
	//resetujemy wartość combobox priorytetu po dodaniu zadania
	
	if (prioritySelect) prioritySelect.value = "";

    loadTasks();
  } catch (error) {
    logError(error);
  }
  
  
}

// PUT /api/tasks/{id}
async function updateTask(id) {
  const nazwa = document.getElementById(`nazwa-${id}`).value;
  const isCompleted = document.getElementById(`completed-${id}`).checked;

//pobieramy aktulaną wartość priorytetu z comboboxa konkretnego zadania na liście
const prioritySelect = document.getElementById(`priority-${id}`);
const priorityValue = prioritySelect && prioritySelect.value !==  "" ? parseInt(prioritySelect.value) : null;

 


  const task = {
    id: id,
    nazwa: nazwa,
	isCompleted: isCompleted,
	
	// dołączamy priority do obiektu aktualizającego zadanie
	
	priority: priorityValue,
  };

  const url = `${apiUrl}/${id}`;

  try {
    logRequest("PUT", url, task);

    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(task),
    });

    const data = await readResponse(response);

    logResponse(response, data);

    if (!response.ok) {
      return;
    }

    loadTasks();
  } catch (error) {
    logError(error);
  }
}

// DELETE /api/tasks/{id}
async function deleteTask(id) {
  const url = `${apiUrl}/${id}`;

  try {
    logRequest("DELETE", url);

    const response = await fetch(url, {
      method: "DELETE",
    });

    const data = await readResponse(response);

    logResponse(response, data);

    if (!response.ok) {
      return;
    }

    loadTasks();
  } catch (error) {
    logError(error);
  }
}

// Wyświetlanie listy zadań
function showTasks(tasks) {
  const container = document.getElementById("tasks");

  container.innerHTML = "";

  tasks.forEach((task) => {
    const div = document.createElement("div");

    div.className = "task";
	
	const isDone = task.isCompleted || false;
	
	//Sprawdzamy priorytet zadania pobranegoo z api (jeśli null/undefinied, dodajemy pusty string) 
	
	const currentPriority = task.priority !== undefined && task.priority !== null ? task.priority : "";
    div.innerHTML = `
            <span>ID: ${task.id}</span>
			
			<input 
			type="checkbox"
			id="completed-${task.id}"
			${isDone ? "checked" : ""} 
            onchange="updateTask(${task.id})"
        >
			

            <input
                id="nazwa-${task.id}"
                value="${task.nazwa}"
            style="${isDone ? 'text-decoration: line-through; color: gray;' : ''}"
			
			
			
			
        >
		
		<select id="priority-${task.id}" onchange="updateTask(${task.id})">
    <option value="" ${currentPriority === "" ? "selected": ""}>Brak priorytetu</option> 
    <option value="1" ${currentPriority === 1 ? "selected": ""}>Niski</option>
    <option value="2" ${currentPriority === 2 ? "selected": ""}>Średni</option>
    <option value="3" ${currentPriority === 3 ? "selected": ""}>Wysoki</option> 
	</select>
    <button onclick="updateTask(${task.id})"
            <button onclick="updateTask(${task.id})">
                Zapisz
            </button>

            <button onclick="showDetails(${task.id})">
                Szczegóły
            </button>

            <button onclick="deleteTask(${task.id})">
                Usuń
            </button>
        `;

    container.appendChild(div);
  });
}

// Odczyt odpowiedzi API
async function readResponse(response) {
  const text = await response.text();

  if (!text) {
    return null;
  }

  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

// Log zapytania
function logRequest(method, url, body = null) {
  const log = document.getElementById("apiLog");

  const bodyText = body ? JSON.stringify(body, null, 2) : "brak";

  log.innerHTML =
    `
        <div class="log request">
            <strong>ZAPYTANIE</strong>
            <br>
            Metoda: ${method}
            <br>
            URL: ${url}
            <br>
            Body:
            <pre>${bodyText}</pre>
        </div>
    ` + log.innerHTML;
}

// Log odpowiedzi
function logResponse(response, data) {
  const log = document.getElementById("apiLog");

  const className = response.ok ? "success" : "error";

  let dataText;

  if (data === null) {
    dataText = "brak treści";
  } else if (typeof data === "string") {
    dataText = data;
  } else {
    dataText = JSON.stringify(data, null, 2);
  }

  log.innerHTML =
    `
        <div class="log ${className}">
            <strong>ODPOWIEDŹ</strong>
            <br>
            Status: ${response.status} ${response.statusText}
            <br>
            OK: ${response.ok}
            <br>
            Body:
            <pre>${dataText}</pre>
        </div>
    ` + log.innerHTML;
}

// Błąd sieci / JavaScript
function logError(error) {
  const log = document.getElementById("apiLog");

  log.innerHTML =
    `
        <div class="log error">
            <strong>BŁĄD</strong>
            <br>
            ${error.message}
        </div>
    ` + log.innerHTML;
}

document.getElementById("newTitle").addEventListener("keydown", function(event) {
  if (event.key === "Enter") {
    event.preventDefault(); // Zapobiega domyślnym akcjom przeglądarki
    addTask(); // Wywołuje Twoją funkcję dodającą zadanie
  }
  

});

  // DELETE /api/ToDoList (usunięcie wszystkich zadań)
async function deleteAllTasks() {
  if (!confirm("Czy na pewno chcesz usunąć wszystkie zadania?")) {
    return; // Anuluje, jeśli użytkownik kliknie "Anuluj" w oknie potwierdzenia
  }

  const url = apiUrl;

  try {
    logRequest("DELETE", url);

    const response = await fetch(url, {
      method: "DELETE",
    });

    const data = await readResponse(response);

    logResponse(response, data);

    if (!response.ok) {
      return;
    }

    // Odśwież listę po pomyślnym usunięciu (będzie pusta)
    loadTasks();
  } catch (error) {
    logError(error);
  }
}