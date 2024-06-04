VAR initialize = 0
VAR score = 0

->start
=== start ===
Kumusta! Naramdaman ko ang iyong presensya sa lugar na ito. May mga nangyari sa lugar na ito na aking mga narinig. Maaari mo bang Ikwento muli sakin ang mga pangyayari? #speaker:Tagapag-bantay #portrait:tagapag_bantay
* [Opo]
~ initialize = initialize + 2
    -> question1
* [Sa susunod nalang po, mag lilibot muna ako.]
~ initialize = initialize + 1
    -> END

=== question1 ===
Bakit nangangailangan ng paliwanag si Ibarra ukol sa kanyang ama?
* [Dahil nais niyang makatulong sa kanyang ama.]
~ score = score + 0
    -> question2
* [Para magkaruon ng linaw at pag-unawa sa nakaraan ng kanyang pamilya.]
~ score = score + 1
    -> question2
* [Ito'y naging interesante lamang sa kanya.]
~ score = score + 0
    Hindi naman yan ang aking narinig.
    -> question2


=== question2 ===
Ano ang implikasyon ng kuwento ng tenyente tungkol sa artilyero sa kwento ni Ibarra?
* [Nagbibigay ito ng masusing paglalarawan sa ugali ng artilyero.]
~ score = score + 0
    Hindi ba si Crisostomo ang lalaking galing Europa?
    -> question3
* [Ito'y nagiging halimbawa ng pagbabago ng kanyang ama.]
~ score = score + 1
    -> question3
* [Walang koneksiyon ang kuwento sa kwento ni Ibarra.]
~ score = score + 0
    -> question3

=== question3 ===
Paano naiugma ang musika ng biyulin sa eksena ng Fonda de Lala sa damdamin ni Ibarra? 
* [Ito'y nagbigay-linaw sa eksena ngunit walang epekto kay Ibarra.]
~ score = score + 0
    -> question4
* [Nagdulot ito ng malalim na emosyon at nagbigay-daan sa introspeksiyon ni Ibarra.]
~ score = score + 1
    -> question4
* [Hindi ito nakakatulong sa eksena at nagdulot ng pang-aantok.]
~ score = score + 0
    -> question4
    
-> question4
=== question4 ===
Ano ang maaaring epekto ng kulay ng mukha ni Kapitan Tiago sa kanyang karakter?
* [Ito ay nagpapakita ng kanyang kalusugan.]
~ score = score + 0
    -> question5
* [Nagbibigay ito ng impormasyon tungkol sa kanyang personalidad.]
~ score = score + 1
    -> question5
* [Wala itong kahalagahan sa kwento.]
~ score = score + 0
    -> question5

=== question5 ===
Bakit mahalaga para kay Ibarra na malaman ang nangyari sa kanyang ama?
* [Ito'y nagbibigay daan sa pag-unlad ng kwento.]
~ score = score + 1
    -> question6
* [Nagbibigay ito ng pagkakataon para maiparating ang galit kay Padre Damaso.]
~ score = score + 0
    -> question6
* [Para maparusahan ang mga may sala sa nangyari sa kanyang ama.]
~ score = score + 0
    -> question6
    
-> question6
=== question6 ===
Ano ang reaksyon ni Ibarra sa pangyayaring naganap sa Fonda de Lala?
* [Wala siyang naramdamang emosyon.]
~ score = score + 0
    -> question7
* [Masaya siya at naging maligaya sa eksena.]
~ score = score + 0
    -> question7
* [Ipinakita niyang labis na naapektohan at nagtanong ng maraming katanungan.]
~ score = score + 1
    -> question7
    
-> question7
=== question7 ===
Paano naiiba ang reaksyon ni Ibarra sa kuwento ng tenyente kumpara sa inaasahan ng marami?
* [Hindi siya nagreact dahil wala siyang pakialam sa kwento ng tenyente.]
~ score = score + 0
    -> question8
* [Tila masaya si Ibarra sa naging pahayag ng tenyente.]
~ score = score + 1
    -> question8
* [Ipinakita niyang labis na naapektohan at nagtanong ng maraming katanungan.]
~ score = score + 0
    -> question8
    
-> question8
=== question8 ===
Bakit mahalaga ang musika sa eksena ng Fonda de Lala sa kabuuan ng kwento?
* [Nagbibigay ito ng kulay at damdamin sa eksena.]
~ score = score + 1
    -> question9
* [Ito'y nagbibigay-tuwa lamang sa mga karakter.]
~ score = score + 0
    -> question9
* [Walang kinalaman ang musika sa kwento.]
~ score = score + 0
    -> question9
    
-> question9
=== question9 ===
Ano ang naging epekto ng kuwento ng tenyente sa pagtingin ni Ibarra sa relihiyon?
* [Tumaas ang pagtingin niya sa simbahan.]
~ score = score + 0
    -> question10
* [Nagkaruon siya ng pangamba at pagdududa sa relihiyon.]
~ score = score + 1
    -> question10
* [Wala itong kahalagahan sa kwento.]
~ score = score + 0
    -> question10
    

=== question10 ===
Paano naapektohan ang pananaw ni Ibarra sa ama matapos ang kuwento ng tenyente?
* [Tumindi ang galit niya sa ama.]
~ score = score + 0
    -> correction
* [Nagkaruon siya ng malalim na pang-unawa at awa sa ama.]
~ score = score + 1
    -> correction
* [Wala itong epekto sa kanya.]
~ score = score + 0
    -> correction

    
=== correction ===
{score == 10:
    Nagawa mo siyang i-perpekto at dahil diyan, pwede kang tumuloy
    -> end
    - else:
    Hindi mo nagawang i-perpekto at dahil diyan, uulit ka
    -> end
}

=== end ===
Nakakakuha ka ng {score} na puntos sa 10 na tanong.
-> END


