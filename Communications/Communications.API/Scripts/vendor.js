$(function () {

	$("#register").on("click", function () {

        $('#vendorRegistrationResultSection').attr('hidden', true);
        $('#vendorKey').text("");

		var endpoint = "api/vendors/register";

		var vendorName = $("#vendorName").val();
		var vendorEmail = $("#vendorEmail").val();

		var vendor = {
			DefaultFromAddress: vendorEmail,
			Name: vendorName
		};

		var payload = JSON.stringify(vendor);

        $.ajax({
            type: "POST",
            url: endpoint,
            data: payload,
            contentType: "application/json"
        }).then(function (data) {
            console.log(data.payload);

            $('#vendorRegistrationResultSection').removeAttr('hidden');
            $('#vendorKey').text(data.payload);


        }).catch(function (error) {
            console.log(error);
        });

	});

});