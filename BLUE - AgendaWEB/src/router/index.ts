import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/LoginUsuarioView.vue')
  },
  {
    path: '/cadastro',
    name: 'Cadastro',
    component: () => import('@/views/RegistroUsuarioView.vue')
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('@/views/HomeUsuarioView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/criar-contato',
    name: 'CriarContato',
    component: () => import('@/views/CriarContatoView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/editar-contato/:id',
    name: 'EditarContato',
    component: () => import('@/views/EditarContatoView.vue'),
    meta: { requiresAuth: true },
    props: true
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Guarda de navegação
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  } else if ((to.name === 'Login' || to.name === 'Cadastro') && authStore.isAuthenticated) {
    next('/home')
  } else {
    next()
  }
})

export default router
