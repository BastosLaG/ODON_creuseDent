<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$stepName = $_POST["StepName"];
$dentalDamInstallID = $_POST["dentalDamInstall"]; // Foreign key referencing the dentaldaminstall

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Use prepared statement to avoid SQL injection
$sql = "SELECT StepID, StepDoneNumber FROM installstep WHERE StepName = ? AND dentalDamInstall = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("si", $stepName, $dentalDamInstallID); // "si" means string and integer parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // If the step already exists, we fetch the StepDoneNumber and increment it
    $row = $result->fetch_assoc();
    $stepDoneNumber = $row['StepDoneNumber'];
    
    // Update the existing step with the incremented StepDoneNumber
    $sql2 = "UPDATE installstep SET StepDoneNumber = ? WHERE StepID = ?";
    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("ii", $stepDoneNumber, $row['StepID']);
    
    if ($stmt2->execute()) {
        echo "Install step exists, Success : " . $row["StepID"];
    } else {
        echo "Error updating StepDoneNumber: " . $stmt2->error . "<br>";
    }
    
    $stmt2->close(); // Close the second statement
} else {
    // If the step doesn't exist, we create a new entry
    $stepDoneNumber = 0;
    
    // Insert the new step
    $sql3 = "INSERT INTO installstep (StepName, StepDoneNumber, dentalDamInstall) VALUES (?, ?, ?)";
    $stmt3 = $conn->prepare($sql3);
    $stmt3->bind_param("sii", $stepName, $stepDoneNumber, $dentalDamInstallID);
    
    if ($stmt3->execute()) {
        // Retrieve the inserted StepID
        $stepID = $conn->insert_id;
        echo "New install step created with StepID, Success : " . $stepID;
    } else {
        echo "Error inserting new step: " . $stmt3->error . "<br>";
    }
    
    $stmt3->close(); // Close the third statement
}

$stmt->close(); // Close the first statement
$conn->close(); // Close the connection
?>
