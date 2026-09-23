const apiUrl = "https://localhost:7232/ToDoList";

// GET /api/tasks
async function loadTasks() {
  const url = apiUrl;

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

  const task = {
    nazwa: nazwa,
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

    loadTasks();
  } catch (error) {
    logError(error);
  }
}

// PUT /api/tasks/{id}
async function updateTask(id) {
  const nazwa = document.getElementById(`nazwa-${id}`).value;

  const task = {
    id: id,
    nazwa: nazwa,
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

    div.innerHTML = `
            <span>ID: ${task.id}</span>

            <input
                id="nazwa-${task.id}"
                value="${task.nazwa}"
            >

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
