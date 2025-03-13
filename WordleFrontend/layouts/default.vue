<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-toolbar-title>Wordle Game</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn text to="/" exact>
        Home
      </v-btn>
      <v-btn text to="/leaderboard">
        Leaderboard
      </v-btn>
      <template v-if="isAuthenticated">
        <v-btn text to="/profile">
          Profile
        </v-btn>
        <v-btn text @click="logout">
          Logout
        </v-btn>
      </template>
      <template v-else>
        <v-btn text to="/login">
          Login
        </v-btn>
        <v-btn text to="/register">
          Register
        </v-btn>
      </template>
    </v-app-bar>

    <v-main>
      <v-container fluid>
        <slot></slot>
      </v-container>
    </v-main>

    <v-footer app color="primary" dark>
      <span>&copy; {{ new Date().getFullYear() }} Wordle Game</span>
      <v-spacer></v-spacer>
      <v-btn text href="https://github.com/yourusername/wordle-game" target="_blank">
        GitHub
      </v-btn>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: 'DefaultLayout',
  computed: {
    isAuthenticated() {
      return this.$store.state.auth.isAuthenticated
    }
  },
  methods: {
    async logout() {
      await this.$store.dispatch('auth/logout')
      this.$router.push('/login')
    }
  }
}
</script> 