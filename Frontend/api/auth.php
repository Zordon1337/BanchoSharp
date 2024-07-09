<?php
/*
i prefer php when it comes to db
*/
$username = $_GET['username'];
$password = $_GET['password'];
include_once("../db.php");
include_once("../func.php");
UpdateScore($username);
UpdateAccuracy($username);
$sql = "SELECT * FROM users WHERE username = '$username' && password = '$password'";
$result = $db->query($sql);
if($result->num_rows > 0)
{
    while($row = $result->fetch_assoc())
    {
        $userid = $row['userid'];
        $totalscore = $row['totalscore'];
        $accuracy = round((float)$row['accuracy'],2);
        $PP = $row['PP'];
        $resp = "1|$userid|$accuracy|$totalscore|$PP";
        echo $resp;
    }
} else {
    echo "0";
}
