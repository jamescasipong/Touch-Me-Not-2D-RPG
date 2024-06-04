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
Nagkakagulo sa labas kanina at maraming tao ang nasa labas. Saan kaya sila pupunta? 
* [Dahil po kilala si Kapitan Tiyago at mamimigay ng mainit na tinola para sa lahat.]
~ score = score + 1
    Ahh! Oo nga, ayun ang aking narinig. 
    -> question2
* [Siguro po nasusunog na po ang bahay ni kapitan Tiyago kaya po maraming tao sa labas]
~ score = score + 0
    Hindi naman yan ang aking narinig.
    -> question2
* [Dahil po kilala si Kapitan Tiyago at mamimigay ng mainit na tinola para sa lahat.]
~ score = score + 0
    Hindi naman yan ang aking narinig.
    -> question2


=== question2 ===
Nung nasa loob na, sino-sino ang mga tao doon? Tapos meron din daw dumating na galing europa? Sino iyon? 
* [Sila Juan at pedro, tapos yung mulang europa si Tenyente.]
~ score = score + 0
    Hindi ba si Crisostomo ang lalaking galing Europa?
    -> question3
* [May nag rambulan sa labas at damay ang mga kababaihan.]
~ score = score + 0
    Hindi ba si Crisostomo ang lalaking galing Europa?
    -> question3
* [Ang mga pumunta ay sina Padre Damaso, Padre Sibyla, Kawal, at ang Tenyente. At ang taong dumating mula sa Europa ay si Crisostomo Ibarra.]
    Ahh, Si Crisostomo ang lalaking galing Europa… 
~ score = score + 1
    -> question3

=== question3 ===
Balita ko rin na mainit ang ulo ni padre damaso. Ano ang kanyang pinapahiwatig? 
* [Naiis nis Padre Damaso sa mga paboritong bisita ni kapitan tiyago si Crisostomo Ibara.]
~ score = score + 0
    -> question4
* [Si Padre Damaso ay galit dahil mas pinapaboran ni Chrisostomo Ibarra ang Espanya kaysa sa sariling bansa.]
~ score = score + 1
    -> question4
* [Naiinis si Padre Damaso sa kadahilanan na gusto niya ng sinigang na maraming laman.]
~ score = score + 0
    -> question4
    
-> question4
=== question4 ===
Ano daw ang mga nagustuan ni Ibarra nung siya’s nasa europa
* [Ang sabi ni ibarra ay maganda raw doon, sapagkat maraming mga babae.]
~ score = score + 0
    -> question5
* [Ang sabi ni ibarra, tagumpay ang pangagailangan ng bayan sa euroopa at katulad ng kanilang kalayaan.]
    Sabagay... Ang taglay ng bansa ay nakikita sa kalayaan ng bansa. Dami mong nalalaman iho. 
~ score = score + 1
    -> question5
* [Ang sabi ni ibarra marami daw pwedeng pasyalan.]
~ score = score + 0
    -> question5

-> question5
=== question5 ===
Pwede mo pa ba Ikwento sakin ang mga sumsunod?
* [Oo naman!]
~ score = score + 1
    -> question6
* [Ayaw ko na po, Hindi ko na natatandaan ang mga pangyayari.]
~ score = score + 0
    -> question6

-> question6
=== question6 ===
Alam mo? Nagugutom ako. Ay!! Nung naghapunan kayo? Mukang masarap ang ulam niyo! Ano nga ba iyon?
* [Tinola po! Kaso panget na mga laman na nakuha ni padre damaso kaya nagalit rin siya.]
~ score = score + 1
    -> question7
* [Sinigang po! Kaso di gaanong maasim kaya nagalit si padre damso.]
~ score = score + 0
    -> question7
* [Abobo po! Kaso masadong maalat kaya nagalit si padre damaso.]
~ score = score + 0
    -> question7
    
-> question7
=== question7 ===
Umalis agad daw si ibarra. Saan daw siya pupunta?
* [Sa Gubat po]
~ score = score + 0
    -> question8
* [Sa Binondo po]
~ score = score + 0
    -> question8
* [Sa San Diego po]
~ score = score + 1
    -> question8
    
-> question8
=== question8 ===
Mukang marami kang natutunan sa mga kabanata na ito. Ito’y tulad ng?
* [Ang natutunan ko ay Ibinukas nito ang paksang pang-edukasyon at ipinakita ang kanyang pangarap para sa mas makatarungan at makabuluhang sistema ng edukasyon.]
~ score = score + 0
    -> question9
* [ang natutunan ko ay mas maiging nang sa ibang bansa mag aral para mas maraming matutunan.]
~ score = score + 1
    -> question9
* [Ang pag-aaral ay di mahalaga kasi puwede naman tayong matuto sa ating mga kapaligiran.]
~ score = score + 0
    -> question9
    
-> question9
=== question9 ===
Oo tama ang edukasyon ang pinakamagandang sandata sa pang araw-araw na buhay. Ano pa?
* [Ang pagpapakilala ni Crisostomo Ibarra ay nagdala ng masamang balita mula sa Europa.]
~ score = score + 0
    -> question10
* [Ang hapag-kainan ay nagkaruon ng masiglang kasiyahan at pagsasaya sa buong kabanata.]
~ score = score + 0
    -> question10
* [Ang pagpapakilala ni Crisostomo Ibarra ay nagbibigay-diin sa kanyang pagiging tanyag at kilala sa kanyang ama. ]
~ score = score + 1
    -> question10
    

=== question10 ===
Isa nalang... para mapunayan kong may natutunan ka sa kabanatang ito.
* [Matuto sa kanyang sariling pagganap ng tradisyon at kultura mula sa Europa at pagbibigay halaga sa likas na kabutihan sa ating bayan]
~ score = score + 1
    -> correction
* [Ang pangunahing layunin ni Crisostomo Ibarra sa kabanatang ito ay ang mang-insulto at manakit ng damdamin ng ibang tauhan.]
~ score = score + 0
    -> correction
* [Ang pag-alaala ni Ibarra sa kanyang ama ay nagdudulot ng kanyang pagnanasa na palaguin at paunlarin ang bayan upang mapabuti ang kalagayan ng mga tao.]
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


