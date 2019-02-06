var Kategorije = []
//fetch categories from database
function LoadKategorija(element) {
    if (Kategorije.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/Nalog_Stavke/getArtikalKategorijas',
            success: function (data) {
                Kategorije = data;
                //render catagory
                renderKategorija(element);
            }
        })
    }
    else {
        //render catagory to the element
        renderKategorija(element);
    }
}

function renderKategorija(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Kategorije, function (i, val) {
        $ele.append($('<option/>').val(val.KategorijaID).text(val.NazivKategorije));
    })
}

//fetch products
function LoadArtikal(kategorijaDD) {
    $.ajax({
        type: "GET",
        url: "/Nalog_Stavke/getArtikals",
        data: { 'kategorijaID': $(kategorijaDD).val() },
        success: function (data) {
            //render products to appropriate dropdown
            renderArtikal($(kategorijaDD).parents('.mycontainer').find('select.artikal'), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function renderArtikal(element, data) {
    //render product
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data, function (i, val) {
        $ele.append($('<option/>').val(val.ArtikalID).text(val.NazivArtikla));
    })
}

$(document).ready(function () {
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;
        if ($('#artikalKategorija').val() == "0") {
            isAllValid = false;
            $('#artikalKategorija').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#artikalKategorija').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#artikal').val() == "0") {
            isAllValid = false;
            $('#artikal').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#artikal').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#kolicina').val().trim() != '' && (parseInt($('#kolicina').val()) || 0))) {
            isAllValid = false;
            $('#kolicina').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#kolicina').siblings('span.error').css('visibility', 'hidden');
        }



        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.ak', $newRow).val($('#artikalKategorija').val());
            $('.artikal', $newRow).val($('#artikal').val());

            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#artikalKategorija,#artikal,#kolicina,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#orderdetailsItems').append($newRow);

            //clear select data
            $('#artikalKategorija,#artikal').val('0');
            $('#kolicina').val('');
            $('#orderItemError').empty();
        }

    })

    //remove button click event
    $('#orderdetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;

        //validate order items
        $('#orderItemError').text('');
        var list = [];
        var errorItemCount = 0;
        $('#orderdetailsItems tbody tr').each(function (index, ele) {
            if (
                $('select.artikal', this).val() == "0" ||
                (parseInt($('.kolicina', this).val()) || 0) == 0
                
            ) {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var orderItem = {
                    ArtikalID: $('select.artikal', this).val(),
                    Kolicina: parseInt($('.kolicina', this).val())

                }
                list.push(orderItem);
            }
        })

        if (errorItemCount > 0) {
            $('#orderItemError').text(errorItemCount + " invalid entry in order item list.");
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#orderItemError').text('At least 1 order item required.');
            isAllValid = false;
        }

        if ($('#brojNaloga').val().trim() == '') {
            $('#brojNaloga').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#brojNaloga').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#datumNaloga').val().trim() == '') {
            $('#datumNaloga').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#datumNaloga').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var data = {
               
                BrojNaloga: $('#brojNaloga').val().trim(),
                DatumNalogaString: $('#datumNaloga').val().trim(),
            

                StavkeNalogaProizvodnjes: list
            }

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: '/Nalog_Stavke/save',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == true) {
                        alert('Successfully saved');
                        //here we will clear the form
                        list = [];
                        $('#brojNaloga,#datumNaloga').val('');
                        $('#orderdetailsItems').empty();
                    }
                    else {
                        alert('Error');
                    }
                    $('#submit').val('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').val('Save');
                }
            });
        }

    });

});

LoadKategorija($('#artikalKategorija'));