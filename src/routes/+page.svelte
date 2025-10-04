<script>
  import { onMount } from "svelte";

  let rows = [];

    onMount(() => {
        fetch("/api/trash")
        .then(res => res.json())
        .then(data => {
            rows = data
        });

        fetch("/api/auth/session", {
        credentials: "include"
        });

    })

</script>

<h1>Welcome to Aplikasi Sampah</h1>
<p>Buanglah sampah pada tempatnya! Jangan jorok dan jaga lingkungan!</p>

<table>
    <thead>
        <tr>
            <th>Id</th>
            <th>Nama Sampah</th>
            <th>Harga</th>
            <th>Organic</th>
            <th>Gambar</th>
            <th>Last Update</th>
        </tr>
    </thead>
    <tbody>
        {#each rows as row}
            <tr>
                <td>{row.trashNID}</td>
                <td>{row.trashName}</td>
                <td>{row.price}</td>
                <td>{row.organic ? "Yes" : "No"}</td>
                <td>
                    <img width="100" src={row.image} alt={row.trashName} />
                </td>
                <td>{new Date(row.lastUpdate).toLocaleString()}</td>
            </tr>
        {/each}
    </tbody>
</table>
