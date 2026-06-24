<template>
  <div class="layout">
    
    <!-- Sidebar -->
    <aside class="sidebar">
      <h2>Payer</h2>
      <ul>
        <li>Dashboard</li>
        <li>Facilities</li>
        <li>Reminders</li>
        <li>Audit</li>
      </ul>
    </aside>

    <!-- Main Content -->
    <div class="main">
      
      <!-- Navbar -->
      <header class="navbar">
        <h3>Prior Authorization</h3>
      </header>

      <!-- Page Content -->
      <main class="content">
        <router-view />
      </main>

    </div>
  </div>
</template>

<style scoped>
.layout {
  display: flex;
}

.sidebar {
  width: 220px;
  background-color: var(--primary);
  color: white;
  height: 100vh;
  padding: 20px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
}

.sidebar li {
  margin: 15px 0;
  cursor: pointer;
}

.navbar {
  background-color: var(--white);
  padding: 15px;
  border-bottom: 1px solid #ddd;
}

.main {
  flex: 1;
}

.content {
  padding: 20px;
}
</style>