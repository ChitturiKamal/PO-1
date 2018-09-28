 
    $(document).ready(function () {
        $.ajax({
            url: "/api/teissues/PostIssue",
            type: "Post",
            data: '{"projectId":"1"}',  
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                alert('Hello');
                alert(data[0].Uniqueid);
            },
            error: function () { alert('error'); }
        });
    });
 