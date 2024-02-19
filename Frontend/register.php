<?php
if(isset($_POST['username'])&&isset($_POST['password']))
{
    include("db.php");
    $username = mysqli_escape_string($db,$_POST['username']);
    $password = md5(mysqli_escape_string($db,$_POST['password']));
    $sql1 = "SELECT * FROM users";
    $sql1 = "SELECT COUNT(*) as count FROM users";
    $result = $db->query($sql1);
    $row = $result->fetch_assoc();
    $rows = $row['count'] + 1;
    $stmt = $db->prepare("INSERT INTO `users`(`userid`, `username`, `password`, `totalscore`, `accuracy`, `PP`) VALUES (?, ?, ?, '0', '1', '0')");
    $stmt->bind_param("sss", $rows, $username, $password);
    $stmt->execute();
    $stmt->close();
    Header("Location: index.php");
    die();
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Register</title>
    
</head>
<body style="background-color:black;color:white;text-align:center;font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;">
    <h1>Register your account</h1>
    <form action="register.php" method="POST">
        <p>Username</p>
        <input name="username"/>
        <p>Password</p>
        <input name="password" type="password"/>
        <br/>
        <br/>
        <input type="submit" value="create account"/>
    </form>
</body>
</html>