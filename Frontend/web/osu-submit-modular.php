<?php
$key = "h89f2-890h2h89b34g-h80g134n90133";
$iv = $_POST['iv'];
$score = $_POST['score'];
include("../func.php");
echo "error: no";
$scoredec = DecryptScore($iv,$key,$score);
$completed = 2;
if(isset($_POST['x']))
{
    $completed = $_POST['x']; // user didin't finish the map, we won't submit
}
error_log($scoredec);
if($completed = 2)
{
    InsertScore($scoredec);
}
